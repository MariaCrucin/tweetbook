24. Create an API SDK with Refit

Se instaleaza urmatorul pachet:
	Refit in proiectul TweetBook.sdk

Ne putem BreackPoint-ul unde dorim in Program.cs din  TweetBook.Sdk.Sample

**********************************************************************************

!!! Nu merge aceasta metoda, deoarece Visual Sudio nu poate rula 2 proiecte simultan

Se ruleaza, ca pana acum proiectul TweetBook

Click dr mouse pe TweetBook.Sdk.Sample → Debug
→ Start New Instance – daca dorim sa il ruam cu Debugging
→ Start Without Debugging – daca dorim sa il rula fara Debugging


In meniul de sus dupa butonul rosu de Stop avem cateva sageti:

Step Into (F11), Step Over(F10), StepOut(Shift+F11)

In fereastra de jos, in Autos sau Locals vedem valorile variabilelor 

**********************************************************************************


Click dr pe TweetBook (project) → Open in Terminal

in terminalul Developer PowerShell
	
TweetBook > dotnet run

im deschid o fereastra de browser si in cazul meu paste acolo https://localhost:7155/swagger/index.html

Pentru a opri executia in terminal: Ctrl + C

In Fereastra de Startup Projects de sus aleg TweetBook.Sdk.Sample si apas butonul de Start (ca pana acum)

In meniul de sus dupa butonul rosu de Stop avem cateva sageti:

Step Into (F11), Step Over(F10), StepOut(Shift+F11)

In fereastra de jos, in Autos sau Locals vedem valorile variabilelor 
