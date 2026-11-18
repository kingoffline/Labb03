# labb3
En WPF-applikation för att skapa frågepaket och spela quiz.

## Varför jag byggde projektet så här
Jag ville prioritera en lösning som var
lätt att förstå, underhålla och möjlig att förklara. Därför utgick
jag från mönstren vi gått igenom i kursens code-alongs.

### JSON i en separat tjänst
All läsning och skrivning av JSON ligger i `QuestionPackStorage` i stället för
i ViewModels. Filen ligger bredvid programmet, vilket gjorde den lätt att
hitta och inspektera under utvecklingen.