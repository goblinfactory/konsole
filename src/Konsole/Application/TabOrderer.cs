using System.Collections.Generic;
using System.Linq;

namespace Konsole
{
    /// <summary>
    /// sets the tab ordering of controls. 
    /// </summary>
    /// <remarks>
    /// If you have 6 controls A, B, C, D,E, F with tab orders A=5, B, C=3, D,E=2, F then final tab order should be??
    /// simple rule, take all the controls with numbers first, they go first...
    /// then the rest in order of their receipt,
    /// this allows for fixed position fields supplemented with dynamic controls afterwards.
    /// dynamically added controls receive tab numbers starting from 1000 to keep things simple, avoid tab clashes with 
    /// other controls being added alter in fixed positions.
    /// A=5, B, C=3, D,E=2, F, G=3 becomes
    /// E=2, C=3, G=3, A=5, B=1000, D=1001, F=1002
    /// </remarks>
    public static class TabOrderer
    {
        public static IControl[] SetTabOrdering(IControl[] controls)
        {
            var list = new List<IControl>();
            var haveTabs = controls.Where(c => c.TabOrder.HasValue).OrderBy(c => c.TabOrder);
            var noTabs = controls.Where(c => !c.TabOrder.HasValue);
            foreach (var c in haveTabs) list.Add(c);
            foreach (var c in noTabs) list.Add(c);
            return list.ToArray();
        }

        public static IConsoleApplication[] SetTabOrdering(IConsoleApplication[] controls)
        {
            var list = new List<IConsoleApplication>();
            var haveTabs = controls.Where(c => c.TabOrder.HasValue).OrderBy(c => c.TabOrder);
            var noTabs = controls.Where(c => !c.TabOrder.HasValue);
            foreach (var c in haveTabs) list.Add(c);
            foreach (var c in noTabs) list.Add(c);
            return list.ToArray();
        }

    }
}
