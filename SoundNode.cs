using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rebbit01
{
    class SoundNode : TreeNode
    {
        string attribute, name;

        public SoundNode(string strName) : base(strName)

        {
            name = strName;

        }
        public SoundNode():base()
        {


        }
        public void setAttr(string attr)
        {
            attribute = attr;
        }
        public string getName()
        {
            return name;
        }


        public string getAttr()
        {
            return attribute;

        }
     
    }

}

