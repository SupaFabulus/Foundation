namespace SupaFabulus.Dev.Foundation.Common.DataModels.Ranges
{
    public class DualRangeFloat : AbstractDualRange<float, RangeFloat>
    {
        public DualRangeFloat()
        {
            _rangeA = new RangeFloat(0f, 1f, 0f);
            _rangeB = new RangeFloat(0f, 1f, 0f);
        }
    }
}