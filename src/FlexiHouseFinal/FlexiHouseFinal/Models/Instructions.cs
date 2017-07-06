using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexihouseRoutinesTest
{
   public class Instructions
    {
        public int shelfid { get; set; }
        public int startslot { get; set; }
        public int endslot { get; set; }
        public string itemname { get; set; }
        public int itemid { get; set; }

        public Instructions()
        { }
        public Instructions(int shelfid, int startslot, int endslot, string itemname)
        {
            this.shelfid = shelfid;
            this.startslot = startslot;
            this.endslot = endslot;
            this.itemname = itemname;
        }

        public Instructions(int shelfid, int startslot, int endslot, string itemname, int itemid)
        {
            this.shelfid = shelfid;
            this.startslot = startslot;
            this.endslot = endslot;
            this.itemname = itemname;
            this.itemid = itemid;
        }
    }
}
