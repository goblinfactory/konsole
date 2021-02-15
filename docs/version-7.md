# Konsole Version 7.0

## Controls

### Keyboard navigation

Describe navigation behavior using the code below as an example; opening a window with 3 panels, `top`, `bot`, `left`, `right`

```csharp

// pseudo code for now

var win = new Window();

var (top, bot) = win.SplitTopBottom("top", "bottom");
var (left, right) = win.SplitLeftRight("left", "right");

left.AddControls(
new CharBox(3, 2, 'x', CharBox.ALPHA_UPPER),
new CharBox(7, 2, 'y', CharBox.ALPHA_UPPER),
new CharBox(11, 2, 'z', CharBox.ALPHA_UPPER) 
);

right.AddControls(
new CharBox(3, 2, '1', CharBox.ALPHA_UPPER),
new CharBox(7, 2, '2', CharBox.ALPHA_UPPER),
new CharBox(11, 2, '3', CharBox.ALPHA_UPPER) 
);

win.Run(QuitKey: Keys.Control.('Q').WithPrompt);

```

**How navigation works**

1. when the windows app starts, by calling `Run`, the following happens
    1. controls in all windows are renderered, all of them are rendered in either inactive or disabled state for all windows, ... 
    1. ... except for first window which will have focus set. (styled as active)
    1. If the first active window has controls that can receive focus, then the first control that can recieve focus is rendered with style = active, and the rest as inactive, and the window itself is rendered as active.
    1. If the first active window has a border, then the border is set as active.
    1. If the first active window has splits, then if the 
    
1. sfsdf