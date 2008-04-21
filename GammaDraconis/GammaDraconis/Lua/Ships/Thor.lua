
shipModel = FBXModel("Resources/Models/Thor", "", 0.125)
shieldModel = FBXModel("Resources/Models/Shield", "", 0.025)
shieldModel.visible = false

mountR = MountPoint()
mountR.location = Coords(0.3, 0, 0)
mountR.weapon = Proto.getWeapon("Cannon")

mountL = MountPoint()
mountL.location = Coords(-0.3, 0, 0)
mountL.weapon = Proto.getWeapon("Cannon")

ship = GameObject()

ship.mass = 800

ship.size = 2

ship.rateL = 5
ship.dragL = 1.2

ship.rateR = 2
ship.dragR = 3

ship.maxHealth = 500;
ship.maxShield = 200;
ship.shieldIncreaseRate = 20;

ship.models:Add(shipModel)
ship.shieldModel = shieldModel

ship.mounts:Add(mountR)
ship.mounts:Add(mountL)

Proto.ship:Add("Thor", ship)