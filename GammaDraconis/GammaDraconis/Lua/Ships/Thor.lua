
shipModel = FBXModel("Resources/Models/Thor", "", 1.25)
shipModel2 = FBXModel("Resources/Models/Thor2", "", 1.25)
shipModel2.lighted = false
shieldModel = FBXModel("Resources/Models/Shield", "", 0.25)
shieldModel.visible = false

mountR = MountPoint()
mountR.location = Coords(3.0, 0, 0)
mountR.weapon = Proto.getWeapon("Cannon")

mountL = MountPoint()
mountL.location = Coords(-3.0, 0, 0)
mountL.weapon = Proto.getWeapon("Cannon")

ship = GameObject()

ship.mass = 800

ship.size = 20

ship.rateL = 8
ship.dragL = 0.9

ship.rateR = 5
ship.dragR = 2.5

ship.relativeLookAt = Vector3(0,0,-300)
ship.relativeLookFrom = Vector3(0,10,48)

ship.maxHealth = 500;
ship.maxShield = 200;
ship.shieldIncreaseRate = 20;

ship.models:Add(shipModel)
ship.models:Add(shipModel2)
ship.shieldModel = shieldModel

ship.mounts:Add(mountR)
ship.mounts:Add(mountL)

Proto.ship:Add("Thor", ship)
