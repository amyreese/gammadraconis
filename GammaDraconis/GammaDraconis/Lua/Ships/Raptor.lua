
shipModel = FBXModel("Resources/Models/Raptor", "", 0.25)
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
ship.dragL = 1.2

ship.rateR = 4
ship.dragR = 3

ship.relativeLookAt = Vector3(0,0,-300)
ship.relativeLookFrom = Vector3(0,8,45)

ship.maxHealth = 500;
ship.maxShield = 200;
ship.shieldIncreaseRate = 20;

ship.models:Add(shipModel)
ship.shieldModel = shieldModel

ship.mounts:Add(mountR)
ship.mounts:Add(mountL)

Proto.ship:Add("Raptor", ship)
