using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Extentions;

namespace Dialogue
{
    /// <summary>
    /// Implementation of the IDButton interface. This actually implements a dialog button.
    /// </summary>
    [Serializable]
    public class DButton : IDButton
    {
        [SerializeField]
        public string Text { get; set; }
        public Action OnButtonClick { get; private set; }
        public IEnumerable<IPresentCondition> Conditions { get; set; }
        public IEnumerable<IEffect> Effects { get; set; }
        public IDPage NextPage { get; set; }

        public DButton()
        {
            Text = "[Error -- Text Not Set]";
            Conditions = Enumerable.Empty<IPresentCondition>();
            Effects = Enumerable.Empty<IEffect>();
            OnButtonClick = () => Effects.ForEach(e => e.Apply());
        }

        /// <summary>
        /// Default Exit button
        /// </summary>
        public static DButton Exit
        {
            get => new DButton()
            {
                Text = "Done."
            };
        }

        public static string DefaultText
        {
            get => "OK.";
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}] => {{{2}}} & {3}", Text, string.Join(",", Conditions), string.Join(",", Effects), (NextPage != null) ? NextPage.Text : "End");
        }
    }
}
