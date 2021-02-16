using System.Collections.Generic;
using System.Linq;

namespace BOT.DATA.Helper
{
    public class DynamicPager
    {
        public IEnumerable<T> DynamicSorting<T>(IEnumerable<T> source, string dataSort, string order)
        {
            IEnumerable<T> resultData = source;
            try
            {
                if (!string.IsNullOrEmpty(dataSort))
                {
                    if (order.ToUpper() == "DESC")
                    {
                        var respSorted = source.OrderByDescending(x => x.GetType().GetProperty(dataSort).GetValue(x, null)).ToList();
                        resultData = respSorted;
                    }
                    else
                    {
                        var respSorted = source.OrderBy(x => x.GetType().GetProperty(dataSort).GetValue(x, null)).ToList();
                        resultData = respSorted;
                    }
                }
                resultData = (from
                              r in resultData.ToList()
                              select r
                              ).ToList();
            }
            catch
            {
                return null;
            }
            return resultData;
        }
    }
}