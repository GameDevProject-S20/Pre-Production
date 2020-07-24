using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dialogue
{
    [Serializable]
    public class DPage : IDPage
    {
        // Properties
        
        [SerializeField]
        public string Text { get; set; }

        [SerializeField]
        public IEnumerable<IDButton> Buttons { get; set; }
        public IDAvatar Avatar { get; set; }

        // Public Methods
        /// <summary>
        /// Retrieves a button given its index.
        /// </summary>
        /// <param name="buttonId"></param>
        /// <returns></returns>
        public IDButton GetButton(int buttonIndex)
        {
            return Buttons.ElementAtOrDefault(buttonIndex);
        }
        /// <summary>
        /// Recursively build a tree of all pages from this page forward, including each option branch
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IDPage> GetPageTree()
        {
            var toRecurse = Buttons.Where(b => b.NextPage != null);

            if (toRecurse.ToArray().Length == 0)
            {
                return new List<IDPage>() { this };
            }
            else
            {
                return Buttons.SelectMany(b => b.NextPage.GetPageTree().Append(this));
            }
        }

        public override string ToString()
        {
            return string.Format("{0}\n\n{1}", Text, (Buttons != null) ? string.Join("\n", Buttons.Select(b => string.Format("-> {0}", b))) : "Buttons null"); ;
        }
    }
}
