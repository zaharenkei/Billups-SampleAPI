Description:

That API contains sevaral endpoints for Rock/Paper/Sciccors/Lizard/Spock game.
It could be used with https://codechallenge.boohma.com/ service to provide minimal functionality.
It was written according to the provided instructions which won't be attached due to restrictions,
but most of the endpoint specifications could be fount there, or by using swagger.


Endpoints:

 - GET /choices 
Provides the list of available choices.

- GET /choice
Makes a random choice selection based on external random generation service.

- POST /play
Accepts a user input as a ChoiceId and check either it wins on randomly selected choice.

- GET /stats
Returns stats for a 10 recently played games within current connection.

- POST /stats/reset
Reset stats for current connection.


How to run:

1) Clone project to your local machine.
2) Open it with preferred IDE and launch it
3) You are ready to go as Swagger will be opened
PS: You could also use a Dockerfile to build an image and run it with docker


Bonus requests:

- Do something fun or creative with the game
You could now add "unfair" query parameter to the /play endpoint.
If request is "unfair", then computer will be able to change his choice to Spock, if it'll make him win.

- Add a scoreboard with 10 most recent results.
You could access the scoreboard via GET /stats endpoint.
It will return the formatted line of Player/Computer score from last 10 games, player's win-rate and a list of detailed game results.

- Allow the scoreboard to be reset
You could do it by calling POST /stats/reset endpoint.
It will reset the stats to zero.

- Allow multiple users to play on the same service.
It was done by binding game results to the connection Id.
That allows for a single user play simultaneously in a different browser tab or window.


Remarks:

- As it was not specified, and to remove hardcoded values, set of possible choices was considered as dynamic,
so all operations with, in fact, enum values were presented via additional IDataProvider abstraction.

- As returning an unlimited amount of resources is a bad practice, pagination were added for possible choices reading.

- For external random number provider Polly could be used to improve stability.
Still, as it seems to be working fine during all manual tests, i assumed that it could be left out of scope.

- As tests could take significant amount of time, i provide them only for the Handlers floder as a sample.


External resources used:

https://medium.com/cheranga/using-asynchronous-fluent-validations-in-asp-net-api-831710b0b9cd