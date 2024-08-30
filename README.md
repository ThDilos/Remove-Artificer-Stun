# Remove-Artificer-Stun
A beginner level rainworld mod

Ok so I made this whole thing in one afternoon\
Idk what to do here so imma just write down my experience

First I followed the tut from [https://www.youtube.com/watch?v=JG9cyL5FW90&t=255s] to set up the basic environment\
Including:
1. Template Mod [https://github.com/NoirCatto/RainWorldRemix/tree/master/Templates/TemplateMod]
2. DNSpy
3. VS Dependencies\
Things I did differently:\
When adding dependencies, manually deleting those lines [As illustrated in the video] from the file thingy cannot fix the errors, you need to manually delete those dependencies from the Solution Explorer in Visual Studio.

Then, I scoped the source code abit, [Locaiton of the source code is found in the Rainworld modding wiki]\
found the method I want, and achieved the main function in first try\
Since there is literally only one function I want to achieve, there is no need to apply a Hooks in an additional file.\
I just add the lines into the main file and delete the hook right away lmao

And last I wanted to add an option menu so you can toggle the function on and off\
That took me another hour to trial and error, feels like Java aws JButton JPanel stuff all over again

One thing bothered me the longest is that, in the override initialize function, you must not include the auto-suggested initialize(base) stuff or something like that\
This single line will literally wipe out all the other things you have.\
Literally took me 30 minutes to debug this smh

Big thanks to the tutorial maker, the template mod maker and the rainworld modding wiki
