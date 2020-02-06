using System;

namespace Konsole
{
    public interface ITheme
    {
        // begin-snippet:ITheme
        StyleTheme Theme { get; set; }

        /// <summary>
        /// returns the active theme based on the control's status, Active, Inactive or Disabled or default for non activating controls.
        /// </summary>
        Style Style { get; }

        ControlStatus Status { get; set; }
        // end-snippet
    }
}