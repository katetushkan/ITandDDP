# Frontend Labs

---

The main goal of this course is to give you some basic understanding how the web works.

![5vm258](https://user-images.githubusercontent.com/43992068/150689739-d1da58ff-e8bc-4cf1-85a5-fe7089b846d9.jpg)

---

# Block #1

## Work №1 "Socket"

To accomplish this work u need to create an application for client-server communication with the help of Socket API.

Acceptance criteria:

1. Console applications are allowed
2. U can choose any programming language ur soul desires (C, C++, C#, etc.)
3. Sender's names should be configured after the launch
4. No high-level libraries are allowed

### Variations:

**One**

Initial connection via TCP, message exchange via UDP. Only one to one connection is supported.

Message order control. Each message should contain order-related parameter, so that receiving client could display messages in the right order.

**Two**

Initial connection and message exchange via UDP. Only one to one connection is supported.

Each client stores message history. If the same client connects after a disconnect, messages have to be recovered and displayed by reconnected client. To distinguish between clients, an unique identifier should be generated for each client.

**Three**

Initial connection and message exchange via UDP. Only one to one connection is supported.

Message order control. Each message should contain order-related parameter, so that receiving client could display messages in the right order. Both clients should somehow control that no messages were lost on their way to receiver.

**Four**

Group message exchange via UDP (see multicast and broadcast).

Any client can create a group with unique identifier. Other clients can request an access to the group by identifier. Group creator listens to such invites and can allow or deny access for a specific client. Only after group creator allowed access, corresponding client will receive group messages.

# Block #2

After finishing next 4 works u will have a completed web application. It would be a nice if u have ur own ideas, discuss them with me and we will find a right way to make it happen =)

There are some references for u, as an example:

1. Managing tools, like [Trello](https://trello.com/en)
2. Flash-card CRUD application, [Anki](https://apps.ankiweb.net)
3. Time-management app, [ToDoist](https://todoist.com)
4. Music or video players, [Spotify](https://www.spotify.com/by-ru/), [YouTube](https://www.youtube.com)
5. Messangers, [Telegram](https://telegram.org), [FaceTime](https://support.apple.com/en-us/HT204380)
6. Board games, [Puzzle](https://www.jigsawplanet.com/?rc=play&pid=341f5b46da3d)
7. Calendars, like [Google Calendar](https://calendar.google.com/calendar/u/0/r?pli=1)
8. Photo gallery, like [Google Photos](https://www.google.com/photos/about/)
9. Productivity app, [Forest](https://www.forestapp.cc)

NOTE: U CAN’T CHANGE UR TOPIC AFTER THE CONFIRMATION!!!

## Work №2 "MockUp"

To accomplish this work u need to create a [mock up](https://www.researchgate.net/profile/Jan-Marco-Leimeister/publication/241194731/figure/fig2/AS:392809023590401@1470664377080/Example-of-a-mockup.png) for the application u are going to develop. U should split ur mock up into HTML elements and provide a description for each logical item. Using div tag for each element is not allowed!

Acceptance criteria:

1. GitHub repo is required.
2. README.md file is required with the following information:
    1. Description of ur project.
    2. Link, photo or attachment of ur mock up.
    3. ≥5 main functions with the overview of ur future application

           (**Authorisation is required by default**).

   d. Data models description.


For creating ur mock ups u can use this apps: [Figma](https://www.figma.com), [AdobeXd](https://www.adobe.com/products/xd.html), [Adobe Photoshop](https://www.adobe.com/products/photoshop.html), pen & a piece of paper, and ur imagination.

## Work №3 "HTML/CSS"

`Starting from this moment the latest version of ur application should be deployed and link should be attached to the PR.`

To accomplish this work u need to bring ur mock ups to life. For this part of the work u only need .html and .css files.

Acceptance criteria:

1. GitHub repo is required.
2. PR into the main branch are required.
3. No Libraries, like Bootstrap, are allowed.
4. Ur design should be applicable for the next screen sizes: [370x670 .. 1600x1200]
5. [BEM](https://en.bem.info/methodology/quick-start/) notation is required.
6. Usage of HTML elements should be [semantically](https://www.freecodecamp.org/news/semantic-html5-elements/) correct.
7. Following criteria will show u the grade range:
    1. ≤7 - using basic html tags (`<a>`, `<div>`, `<input>`, `<span>`), [accessibility A](https://www.accessiblemetrics.com/blog/what-are-the-levels-of-wcag-compliance/)
    2. [8..10] - using html tags like `<article>`, `<section>`, `<main>`, `<aside>`, etc., using custom fonts, using css variables, add theme’s changing, [accessibility A, AA.](https://www.accessiblemetrics.com/blog/what-are-the-levels-of-wcag-compliance/)


## Work №4 "JS functionality"

To accomplish this work u need to add interaction with the help of .js files. For this part of the work u need to implement 3 functions from the list mentioned in ur README.md file, these functions should be related to the user interaction (No 3rd party API, for now).

P.S. Functions are gonna be chosen by me😈

Acceptance criteria:

1. GitHub repo is required.
2. PR into the main branch are required.
3. No JS Libraries and Frameworks, like React, Vue, Angular are allowed.
4. Clean project structure is required (do not place all ur code inside of one file).
5. If ur application has a CRUD functionality, it should be covered.
6. Functions for business rules should be implemented.
7. Following criteria will show u the grade range:
    1. ≤7 - form processing, input validation, filtering, sorting.
    2. [8..10] - drag&drop, working with events, some interesting sorting, working with WebRTC, etc.


NOTE: Pls, think about structure of ur project, its scalability and data it is going to process.

## Work №5 "API Integration"

To accomplish this work u need to create modules to work with 3rd party API and integrate ur app with them. It can be Google Calendar, Google Disk, Firebase Services, Mongo DB, AWS S3, Yandex.Maps, GitHub API and so on, whatever ur application needs.’

These modules should be independent, which means that I can add it to any application I want to, and it’s gonna work. U need to create files with functions(like API, Validator, [DTO](https://en.wikipedia.org/wiki/Data_transfer_object)), then u should be able to import it any place u need this functionality and use it.

Acceptance criteria:

1. GitHub repo is required.
2. PR into the main branch are required.
3. No JS Libraries and Frameworks, like React, Vue, Angular are allowed.
4. Clean architecture is required.
5. Authorisation functionality is required.

> The 5th work is gonna be ur final, which is mean that all the functionality requried at the beginning should be implemented.
