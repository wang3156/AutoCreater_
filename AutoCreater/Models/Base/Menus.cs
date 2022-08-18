using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Base
{
    public class Menus
    {
        public Guid ID { get; set; }

        public string MenuName { get; set; }

        public string MenuExplain { get; set; }

        public string MenuUrl { get; set; }

        public bool IsActive { get; set; }

        public Guid? ParentID { get; set; }

        public int _Index { get; set; }

    }
}
