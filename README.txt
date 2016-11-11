------------------= TwitchWorld 0.1a =-------------------------

-----------------------= ABOUT =-------------------------------

TwitchWorld is a widget integrated with Twitch.tv that allows
you and your followers to put the flags on your own planet Earth.

----------------------= FEATURES =-------------------------------

* Fully controllable via twitch chat
* Automatic & manual Earth rotation
* Customizable flag colors per role
* Colors as RGB or by name (with typo tolerance)
* Import/Export JSON

-----------------------= SETUP =-------------------------------

Before launching the application, setup the following parameters in 'config.txt':

twitch.streamer: your Twitch nickname
twitch.bot: your Twitch bot nickname

twitch.api.access_token:
Open the following link (CAUTION! You have to be authorized with your main account)
https://api.twitch.tv/kraken/oauth2/authorize?response_type=token&client_id=q6batx0epp608isickayubi39itsckt&redirect_uri=https://twitchapps.com/tmi/&scope=user_read%20channel_editor%20channel_subscriptions%20channel_check_subscription
Copy & paste the value under "Use the following password to login to chat"

twitch.irc.oauth:
Open the following link (CAUTION! You have to be authorized with your Twitch bot, not your main account - you can use browser incognito mode to avoid logouting from main Twitch account)
https://api.twitch.tv/kraken/oauth2/authorize?response_type=token&client_id=q6batx0epp608isickayubi39itsckt&redirect_uri=https://twitchapps.com/tmi/&scope=chat_login
Copy & paste the value under "Use the following password to login to chat"

----------------------= COMMANDS =-----------------------------

TwitchWorld is controlled via twitch chat with following commands:

!flag - Display flag location of calling user

!flag *PLACE* - Put the flag in arbitrary location
Place may be whatever - continent, country, city, even street, but recommended accuracy is city.
You may want to specify country & city, if flag is placed into another town with the same name.
Examples:
!flag Riga
!flag Moscow Idaho

!flag count - Display total amount of flags

!flag remove - Remove calling user's flag
!flag delete - Same as !flag remove

!flag clear - Remove all flags (* This command is only available to streamer)

!flag color *ROLE* *COLOR* - Set the flag color for specified roles (* This command is only available to streamer)
Available roles are: streamer, sub, mod, viewer
Color can be specified by name or as RGB value
Examples:
!flag color streamer #ff0000
!flag color mod yellow

-----------------------= NOTES =-------------------------------

Thank you for participating in TwitchWorld alpha testing.

In case you have encountered some bugs, found something inconvenient
or just have some suggestions of any kind, please contact me at:

Twitch: https://www.twitch.tv/XLIII
Twitter: https://twitter.com/followXLIII
E-mail: XLIII@mail.com
