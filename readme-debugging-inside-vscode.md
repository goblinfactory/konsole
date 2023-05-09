# Debugging inside Visual studio code

If you are debugging some code that uses Konsole inside VSCode IDE and you are relying on visually inspecting the output written to Console by Konsole, then you will need to use an external terminal  for debugging since the integrated terminal does not support all the .NET console commands like Cursor positioning that Konsole requires.

In order to debug a project using Konsole please add `externalTerminal` to your `launch.json` as  described here; [OmniSharp/omnisharp-vscode#2336](https://github.com/OmniSharp/omnisharp-vscode/issues/2336)

If you don't add this to your lauch json, then you may experience the following exception, 

```ruby
ex:[System.IO.IOException: the handle is invalid]
```

as reported by @mnsrulz here 

https://github.com/goblinfactory/konsole/issues/92

Adding the externalTerminal does what it says, and allows you to debug happily using VSCode.woot...

Happy debugging,

Alan