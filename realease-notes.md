##release notes

- new very simple TestData `Konsole.Testing.TestData` class to help when building and prototyping console utilities and applications.
- renamed MockConsole to TestConsole. (It's not a mock, it's a real active class with important behavior to help build testable code.)
- Split IConsole into IWriteLine and IConsole to allow for being more specific about your dependancies, .i.e. what exactly your class depend on when you state you require a IWriteLine as a dependancy, tells you a lot more than 'I Need access to the console'. (Most classes often simply need acess to write a few bits and bobs to the console, in which case I expect that IWriteLine will be the most commonly used interface.) 
