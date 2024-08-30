#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace SupaFabulus.Dev.Foundation.EditorTools.Common
{
    
    public struct EditorUIControlTextureSet
    {
        private bool _loaded;
        private Texture2D _normal;
        private Texture2D _hover;
        private Texture2D _active;

        public void Load (string normal, string hover, string active)
        {
            EditorGUIUtils.LoadIcon(ref _normal, normal);
            EditorGUIUtils.LoadIcon(ref _hover, hover);
            EditorGUIUtils.LoadIcon(ref _active, active);
            _loaded = true;
        }

        public bool Loaded => _loaded;
        public Texture2D Normal => _normal;
        public Texture2D Hover => _hover;
        public Texture2D Active => _active;

        public GUIStyle ApplyToStyle(GUIStyle s)
        {
            s.normal.background = _normal;
            s.hover.background = _hover;
            s.active.background = _active;
            return s;
        }
    }

    [Serializable]
    public abstract class AbstractEditorIconSet<TIconID>
    {
        
        [SerializeField]
        protected bool _loaded;
        public bool Loaded => _loaded;

        protected Dictionary<TIconID, Texture2D> _iconTable = new();

        public abstract Texture2D[] DesignatedIconReferences { get; }

        public virtual void LoadOrderedIcons(string[] paths)
        {
            Texture2D[] refs = DesignatedIconReferences;
            LoadIconsToReferences(refs, paths);
        }


        public Texture2D IconFromTable(TIconID id)
        {
            if (_iconTable.ContainsKey(id))
            {
                return _iconTable[id];
            }

            return null;
        }

        public void LoadIconsToTable(TIconID[] ids, string[] paths)
        {
            if (ids == null || paths == null || ids.Length != paths.Length) return;
            
            int i;
            int c = paths.Length;
            string path;
            TIconID id;
            Texture2D txt;

            for (i = 0; i < c; i++)
            {
                id = ids[i];
                path = paths[i];
                txt = null;
                LoadIcon(ref txt, path);
                AddIcon(id, txt);
            }
        }
        
        public void LoadIconsToReferences(Texture2D[] refs, string[] paths)
        {
            if (refs == null || paths == null || refs.Length != paths.Length) return;
            
            int i;
            int c = paths.Length;
            string path;
            Texture2D txt;

            for (i = 0; i < c; i++)
            {
                txt = refs[i];
                path = paths[i];
                LoadIcon(ref txt, path);
            }
        }

        public virtual void LoadIcon(ref Texture2D t, string iconFileName)
        {
            if (iconFileName != null)
            {
                EditorGUIUtils.LoadIcon(ref t, iconFileName);
            }
        }

        public virtual void AddIcon(TIconID id, Texture2D icon)
        {
            if (icon != null && !_iconTable.ContainsKey(id))
            {
                _iconTable.Add(id, icon);
            }
        }

        public virtual void RemoveIcon(TIconID id)
        {
            if (_iconTable.ContainsKey(id))
            {
                _iconTable.Remove(id);
            }
        }

        public virtual void ClearIconTable()
        {
            foreach (KeyValuePair<TIconID, Texture2D> entry in _iconTable)
            {
                Texture2D t = entry.Value;
                _iconTable[entry.Key] = null; 
                UnityEngine.Object.DestroyImmediate(entry.Value);
            }
            _iconTable.Clear();
        }
    }

    [Serializable]
    public class EditorUIControlIconSet : AbstractEditorIconSet<string>
    {
        
        [SerializeField]
        protected Texture2D _clear;
        [SerializeField]
        protected Texture2D _defaults;
        [SerializeField]
        protected Texture2D _read;
        [SerializeField]
        protected Texture2D _write;
        [SerializeField]
        protected Texture2D _revert;
        [SerializeField]
        protected Texture2D _modePositive;
        [SerializeField]
        protected Texture2D _modeNegative;
        [SerializeField]
        protected Texture2D _modeNeutral;
        [SerializeField]
        protected Texture2D _optionPositive;
        [SerializeField]
        protected Texture2D _optionNegative;
        [SerializeField]
        protected Texture2D _optionNeutral;

        public void Load(
            string clear = null,
            string defaults = null,
            string read = null,
            string write = null,
            string revert = null,
            string modePositive = null,
            string modeNegative = null,
            string modeNeutral = null,
            string optionPositive = null,
            string optionNegative = null,
            string optionNeutral = null
        )
        {
            EditorGUIUtils.LoadIcon(ref _clear, clear);
            EditorGUIUtils.LoadIcon(ref _defaults, defaults);
            EditorGUIUtils.LoadIcon(ref _read, read);
            EditorGUIUtils.LoadIcon(ref _write, write);
            EditorGUIUtils.LoadIcon(ref _revert, revert);
            EditorGUIUtils.LoadIcon(ref _modePositive, modePositive);
            EditorGUIUtils.LoadIcon(ref _modeNegative, modeNegative);
            EditorGUIUtils.LoadIcon(ref _modeNeutral, modeNeutral);
            EditorGUIUtils.LoadIcon(ref _optionPositive, optionPositive);
            EditorGUIUtils.LoadIcon(ref _optionNegative, optionNegative);
            EditorGUIUtils.LoadIcon(ref _optionNeutral, optionNeutral);
            _loaded = true;
        }
        
        

        
        public override Texture2D[] DesignatedIconReferences => new[]
        {
            _clear,
            _defaults,
            _read,
            _write,
            _revert,
            _modePositive,
            _modeNegative,
            _modeNeutral,
            _optionPositive,
            _optionNegative,
            _optionNeutral
        };

        public Texture2D Clear => _clear;
        public Texture2D Defaults => _defaults;
        public Texture2D Read => _read;
        public Texture2D Write => _write;
        public Texture2D Revert => _revert;
        public Texture2D ModePositive => _modePositive;
        public Texture2D ModeNegative => _modeNegative;
        public Texture2D ModeNeutral => _modeNeutral;
        public Texture2D OptionPositive => _optionPositive;
        public Texture2D OptionNegative => _optionNegative;
        public Texture2D OptionNeutral => _optionNeutral;

    }

    [Serializable]
    public static class EditorGUIUtils
    {

        public const int DEFAULT_BORDER_SIZE = 8;
        
        
        
        static EditorGUIUtils()
        {
            //
        }

        private static bool _isLoaded = false;
        private static readonly string _iconPathBase = "Assets/_INTERNAL/_Source/MainFrame/Assets/_INTERNAL/Images/";
        private static readonly string _iconPathSub_emojis = "emojis/";

        private static readonly string _emojiData;
        
        public static string IconPathBase => _iconPathBase;
        public static string IconEmojiSubDirectoryName => _iconPathSub_emojis;

        private static RectOffset _buttonBorder = null;

        
        private static EditorUIControlIconSet _animCtrlIcons = new();
        private static EditorUIControlIconSet _animGroupIcons = new();
        private static EditorUIControlIconSet _bridgeIcons = new();
        private static EditorUIControlIconSet _mediaTransportIcons = new();
        private static EditorUIControlTextureSet _button = new();
        
        private static GUIStyle _buttonStyle = null;
        
        
        public static EditorUIControlIconSet MediaTransportIcons
        {
            get
            {
                if (!_isLoaded || !_mediaTransportIcons.Loaded) return default;
                return _mediaTransportIcons;
            }
        }
        
        public static EditorUIControlIconSet BridgeIcons
        {
            get
            {
                if (!_isLoaded || !_bridgeIcons.Loaded) return default;
                return _bridgeIcons;
            }
        }
        
        public static EditorUIControlIconSet AnimationControlIcons
        {
            get
            {
                if (!_isLoaded || !_animCtrlIcons.Loaded) return default;
                return _animCtrlIcons;
            }
        }
        
        public static EditorUIControlIconSet AnimationGroupIcons
        {
            get
            {
                return _animGroupIcons;
                if (!_isLoaded || !_animGroupIcons.Loaded) return default;
                return _animGroupIcons;
            }
        }
        
        public static EditorUIControlTextureSet Button
        {
            get
            {
                if (!_isLoaded || !_button.Loaded) return default;
                return _button;
            }
        }

        public static GUIStyle ButtonStyle
        {
            get
            {
                if (!_isLoaded || _buttonStyle == null) return null;
                return _buttonStyle;
            }
        }


        public static void Initialize()
        {
            if (_isLoaded) return;
            
            if (_buttonBorder == null)
            {
                _buttonBorder = new RectOffset
                (
                    DEFAULT_BORDER_SIZE,
                    DEFAULT_BORDER_SIZE,
                    DEFAULT_BORDER_SIZE,
                    DEFAULT_BORDER_SIZE
                );
            }

            
            if (_buttonStyle == null)
            {
                _buttonStyle = new GUIStyle(GUI.skin.button);
                _buttonStyle.border = _buttonBorder;
                _buttonStyle = _button.ApplyToStyle(_buttonStyle);
            }
            
            LoadIcons();

            _isLoaded = true;
        }

        [Serializable]
        private struct CategorizedKeyNameStore
        {
            
        }

        [Serializable]
        private struct IconEntry
        {
            public string Name
            {
                get => _name;
                set => _name = value;
            }

            public string Code
            {
                get => _code;
                set => _code = value;
            }

            public Texture2D Asset
            {
                get => _asset;
                set => _asset = value;
            }

            public IconEntry(string name, string code, Texture2D asset)
            {
                _name = name;
                _code = code;
                _asset = asset;
            }

            [SerializeField]
            private string _name;
            [SerializeField]
            private string _code;
            [SerializeField]
            private Texture2D _asset;
        }

        [Serializable]
        private struct EmojiPlatformSupport
        {
            [SerializeField]
            public bool apple;
            [SerializeField]
            public bool google;
            [SerializeField]
            public bool facebook;
            [SerializeField]
            public bool windows;
            [SerializeField]
            public bool twitter;
            [SerializeField]
            public bool joypixels;
            [SerializeField]
            public bool samsung;
            [SerializeField]
            public bool gmail;
            [SerializeField]
            public bool softbank;
            [SerializeField]
            public bool docomo;
            [SerializeField]
            public bool kddi;
        }

        [Serializable]
        private struct EmojiDataEntry
        {
            [SerializeField]
            public string[] code;
            [SerializeField]
            public string emoji;
            [SerializeField]
            public string name;
            [SerializeField]
            public string category;
            [SerializeField]
            public string subcategory;
            //[SerializeField]
            //public EmojiPlatformSupport support;

            [SerializeField]
            private Texture2D _cache;

            public Texture2D Cache
            {
                get => _cache;
                set => _cache = value;
            }

            public bool IsLoaded => _cache != null;

            [SerializeField]
            private string _id;

            private StringBuilder _sb;

            private const char sp = ' ';
            private const char d = '-';
            
            public string label => 
                name.Replace(':', d)
                    .Replace(',', d)
                    .Replace(sp, d);

            public string id
            {
                get
                {
                    
                    if (_id == null || _id == string.Empty || _id == default)
                    {
                        if (code == null) return null;
                        _sb = new();
                        
                        int i;
                        int c = code.Length;
                        string part;

                        for (i = 0; i < c; i++)
                        {
                            _sb.Append(code[i].ToLower());
                            if (i < c - 1)
                            {
                                _sb.Append('-');
                            }
                        }

                        _id = _sb.ToString();
                        _sb.Clear();
                    }

                    return _id;
                }
            }
        }

        [Serializable]
        private struct EmojiDataSet
        {
            [SerializeField]
            public EmojiDataEntry[] emojis;
        }

        [Serializable]
        private struct EmojiCategory
        {
            [SerializeField]
            public EmojiDataEntry[] emojis;
        }

        [Serializable]
        private struct EmojiSubcategory
        {
            [SerializeField]
            public EmojiDataEntry[] emojis;
        }

        private static Dictionary<string, EmojiDataEntry> _emojiTable = new();

        
        

        public static Texture2D LoadEmoji(string label)
        {
            if (_emojiTable.ContainsKey(label))
            {
                EmojiDataEntry e = _emojiTable[label];
                if (!e.IsLoaded)
                {
                    string url = $"{_iconPathBase}{_iconPathSub_emojis}_all/{e.id}.png";
                    e.Cache = AssetDatabase.LoadAssetAtPath<Texture2D>(url);
                    Debug.Log($"Image: [{e.Cache}]");
                    _emojiTable[label] = e;
                }

                return e.Cache;
            }
            
            //Debug.LogError($"Unable to load Emoji [{label}]");
            return null;
        }
        
        private static void LoadIcons()
        {

            
            string url = _iconPathBase + _iconPathSub_emojis + "emojiData.json";
            //byte[] bytes = File.ReadAllBytes(url);
            string data; //  = Convert.ToString(bytes);

            data = System.IO.File.ReadAllText(url, Encoding.UTF8);
            
            //Debug.Log($"DATA: {data}");

            EmojiDataSet iconCatalog = JsonUtility.FromJson<EmojiDataSet>(data);

            
            string l;
            
            foreach (EmojiDataEntry emoji in iconCatalog.emojis)
            {
                l = emoji.label;
                if (!_emojiTable.ContainsKey(l))
                {
                    _emojiTable.Add(l, emoji);
                }
                //Debug.Log($"'{emoji.label}' ({emoji.id}): {emoji.emoji}");
            }
            

            if (!_mediaTransportIcons.Loaded)
            {
                _mediaTransportIcons.Load
                (
                    "emojis/symbols/274c.png",
                    "emojis/badges/274e.png",
                    "emojis/badges/25b6-fe0f.png",
                    "emojis/badges/23fa-fe0f.png",
                    "emojis/badges/23ea.png",
                    "emojis/symbols/1f518.png",
                    "emojis/other/26ab.png"
                );
            }
            

            if (!_bridgeIcons.Loaded)
            {
                _bridgeIcons.Load
                (
                    "shaded/badge_red_x.bmp",
                    "shaded/plain_doc.bmp",
                    "shaded/download.bmp",
                    "shaded/upload.bmp"
                );
            }
            
            if (!_animCtrlIcons.Loaded)
            {
                _animCtrlIcons.Load
                (
                    
                    "shaded/sweep.bmp",
                    "shaded/defaults.bmp",
                    "shaded/download.bmp",
                    "shaded/upload.bmp",
                    "shaded/blue_arrow_curved_left.bmp",
                    "shaded/settings_gear_record.bmp",
                    "shaded/settings_gear_play.bmp",
                    "shaded/settings_gear_play.bmp",
                    "shaded/badge_round_go.bmp",
                    "shaded/badge_round_red_strike.bmp",
                    "shaded/badge_round_blue_question.bmp"
                );
            }
            
            if (true || !_animGroupIcons.Loaded)
            {
                _animGroupIcons.LoadIconsToTable
                (
                    new []
                    {
                        "record",
                        "actor",
                        "timing",
                        "motion",
                        "state"
                    },
                    new []
                    {
                        "UI/record_white64.png",
                        "UI/actor_white64.png",
                        "UI/timing_white64.png",
                        "UI/motion_white64.png",
                        "UI/settings_white64.png"
                    }
                );
            }

            if (!_button.Loaded)
            {
                _button.Load
                (
                    "UI/UIButton32_dark_normal.png", 
                    "UI/UIButton32_dark_hover.png", 
                    "UI/UIButton32_dark_active.png"
                );
            }
        }
        
        public static void LoadIcon(ref Texture2D t, string iconFileName)
        {
            //Debug.Log($"Loading: {_iconPathBase + iconFileName}");
            t = AssetDatabase.LoadAssetAtPath<Texture2D>(_iconPathBase + iconFileName);
            if (t != null)
            {
                
            }
        }
    }
}
#endif