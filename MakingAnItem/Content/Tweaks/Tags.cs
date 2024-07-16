using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.ItemAPI;

namespace NevernamedsItems
{
    public class Tags
    {
        public static void Init()
        {
            AddTags(new List<int>()
            {
                105, //Fortunes Favour
                272, //Iron Coin
            }, "lucky");
            AddTags(new List<int>()
            {
                289, //Seven Leaf Clover
            }, "very_lucky");
        }
        public static void AddTags(List<int> items, string tag)
        {
            foreach(int i in items) { AlexandriaTags.SetTag(i, tag); }
        }
    }
}
