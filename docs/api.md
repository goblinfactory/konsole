new, Window.Open and Window.OpenBox should have the same constructors

we must update SplitLeftRight as well to match!


```csharp

new Window()
new Window(settings)
new Window(width, height)
new Window(sx, sy, width, height)
new Window(width, height, foreground, background)
new Window(width, height, Style)
new Window(width, height, Theme)

console.Open()
console.Open(settings)
console.Open(width, height)
console.Open(sx, sy, width, height)
console.Open(width, height, foreground, background)
console.Open(width, height, Style)
console.Open(width, height, Theme)

console.Open(foreground, background)
console.Open(Style)
console.Open(Theme)


console.OpenBox(title)
console.OpenBox(title, settings)
console.OpenBox(title, width, height)
console.OpenBox(title, sx, sy, width, height)
console.OpenBox(title, width, height, foreground, background)
console.OpenBox(title, width, height, Style)
console.OpenBox(title, width, height, Theme)
console.OpenBox(title, foreground, background)
console.OpenBox(title, Style)
console.OpenBox(title, Theme)



Window.OpenBox(title)
Window.OpenBox(title, settings)
Window.OpenBox(title, width, height)
Window.OpenBox(title, sx, sy, width, height)
Window.OpenBox(title, width, height, foreground, background)
Window.OpenBox(title, width, height, Style)
Window.OpenBox(title, width, height, Theme)
Window.OpenBox(title, foreground, background)
Window.OpenBox(title, Style)
Window.OpenBox(title, Theme)

```
