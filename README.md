![equili-logo-black-bg](https://cloud.githubusercontent.com/assets/21274043/26372159/c7258f18-3fb1-11e7-902d-1b713b9524ca.png)

Equilibrium is a first person virtual reality adventure game with real-time database connection and a companion web app. Our Firebase connection is integrated with a MySQL database and Sequelize, an object relational mapper for MySQL. The functionality of the web app works closely with encrypted C# data.

## Getting Started
### How to Use the Web App:
- Live link to companion web app: https://equilibrium-game.herokuapp.com/
- The web app has many dynamic features that add to the gameplay experience such as a history log of all items picked up, a WebGL map of the in-game environment, an achievements module, and a wiki of all possible items. It's also got more experimental (in-progress) features such as a slot minigame and a virtual store. Please check it out and give us feedback in the guestbook!
- Live link to Linux (64-bit and 32-bit compatible): https://www.dropbox.com/s/xlbz6arn80uynjf/Linux%2864%2632%29.zip?dl=1
- Live link to Windows (64-bit and 32-bit compatible): https://www.dropbox.com/s/kmpiiteu02jaqq9/Windows%2864%2632%29.zip?dl=1
- MacOSX Build in early alpha, .dmg file currently not available.
- Android Build in early beta, requires Crossplatform D-Pad control integration but functional.
- VR Build, requires Gear VR compatible Android devices, and also requires headset (2016 Edition and above recommended): https://www.dropbox.com/s/5zwek3mrghj8k8u/Equilibrium_Beta7.apk?dl=1

### Prerequisities for Web App:
#### Dependencies
- bcrypt-nodejs
- body-parser
- cookie-parser
- express
- express-handlebars
- express-session
- firebase
- firebase-admin
- gravatar
- method-override
- morgan
- mysql
- nodemon
- passport
- passport-local
- sequelize
- sequelize-cli

### How to Play the Game:
- WASD-Standard controls for Windows/Mac/Linux platforms and press E for interacting with objects. Tab for toggling inventory view.
- VR Only: Look down at a 30-degree angle to move forward. Look up at a 30-degree angle to stop movement. Operate controls with either Samsung Gear VR’s headset control-pad, ‘tap’, to interact with object. Press bluetooth controller to pick up item.

### Prerequisities for Game:
#### For Windows/Mac OS X/Linux (Minimum Requirements)
- OS: Windows 7 and higher (64 bit only) / Mac OS X 10.5 / Ubuntu Linux, version 10.10 or later
- Processor: AMD Phenom II X2 550, 3.1GHz | Intel Pentium G4400, 3.30GHz
- Memory: 1 GB RAM
- Graphics: Integrated Graphics Card
- DirectX: Version 9+
- Network: Local
- Additional Notes: For Windows 7, service pack 1 is required

#### VR (Minimum Requirements)
- Samsung Gear VR
- All requirements that come with Gear VR (Compatible Devices Only)

## Screenshots
### Map of Game Environment (Web App):
![screenshot 2017-05-23 13 25 23](https://cloud.githubusercontent.com/assets/21274043/26374795/636b01ba-3fbb-11e7-9c74-694d09adad49.png)

### Achievements (Web App):
![screenshot 2017-05-23 13 12 29](https://cloud.githubusercontent.com/assets/21274043/26374283/95a2f220-3fb9-11e7-8c26-c837ed7458da.png)

![](http://i.imgur.com/N170leM.gifv)

![](http://i.imgur.com/EkWoQ0n.jpg)
## Technologies Used
### Core Game:
- Unity
- C#
- Android (SDK)
- Oculus (SDK)
- Samsung Gear VR
- AR

### Databases:
- MySQL
- Sequelize
- Firebase (SDK)

### Web App:
- HTML5/CSS3
- JavaScript
- jQuery
- Bootstrap
- Node.js
- Express
- Handblebars
- Three.js
- Collada

## Code Walkthroughs

### Model-View-Controller:
Separating our code into modules made our code much more readable and easy to work with. Although there is some initial investment in planning, the payoff of isolating chunks of code was significant as we can identify errors and, often times, re-use code almost effortlessly.

```
router.get('/',
  itemController.checkItemsList,
  minigameController.checkMinigame,
  wikiController.checkWiki,
  achievementController.checkAchievements,
  storeItemController.checkStoreItems
);
```

## Team BNYY (Contributors)
See the list of [contributors](https://github.com/yoonslee/project2-game/contributors) who participated in this project.

* **Brandon Chang** - https://github.com/karunashi
* **Nathan Miller** - https://github.com/nathanmiller9
* **Yoon Lee** - https://github.com/yoonslee
* **Yen La** - https://github.com/yenla

## License
This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/yoonslee/project2-game/blob/master/LICENSE) file for details.

## Acknowledgments
* Shout out to our mentor and instructor, **Omar Patel** - https://github.com/osp123
* Inspired by the latest and greatest bleeding edge technologies
