namespace BOT.DATA.Helper
{
    public class PagerModel
    {
        ///<summary>
        ///Initial Row Number
        /// </summary>
        public int Offset { get; set; }
        /// <summary>
        /// Number of rows to get
        /// </summary>
        public int Limit { get; set; }
        ///<summary>
        ///Sort Column name
        /// </summary>
        public string sortColumn { get; set; }
        ///<summary>
        ///Sort Column direction (ASC,DESC)
        /// </summary>
        public string sortDir { get; set; }

        public int TotalRows { get; set; }

    }
}
