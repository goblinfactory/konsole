namespace Konsole
{
    public enum ControlStatus { 
        
        Undefined = 0, 

        /// <summary>
        /// control is active and has the focus and is in editing mode, pressing arrow keys moves the cursor.
        /// </summary>
        Active = 1, 

        /// <summary>
        /// control has been selected but is still inactive, i.e. the form (and all controls) are in navigating mode. 
        /// Pressing arrow keys will move the focus to another control, pressing enter or starting to type
        /// will send the keystrokes to the selected control. Before the keys are sent the control "Render" will 
        /// be called updating the status.
        /// </summary>
        InactiveSelected = 2,

        /// <summary>
        /// control does not have focus
        /// </summary>
        Inactive = 3, 
        
        /// <summary>
        /// control is disabled and will not receive focus
        /// </summary>
        Disabled = 4 
    
    }
}