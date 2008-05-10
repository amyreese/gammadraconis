
shipModel = FBXModel("Resources/Models/Raptor", "", 0.25)
shipModel2 = FBXModel("Resources/Models/Raptor2", "", 0.25)
shipModel2.lighted = false
shieldModel = FBXModel("Resources/Models/Shield", "", 0.1875)
shieldModel.visible = false

mountR = MountPoint()
mountR.type = W_TYPE.PRIMARY
mountR.location = Coords(3.0, 0, 0)
mountR.weapon = Proto.getWeapon("Blaster")

mountL = MountPoint()
mountL.type = W_TYPE.PRIMARY
mountL.location = Coords(-3.0, 0, 0)
mountL.weapon = Proto.getWeapon("Blaster")

mountC = MountPoint()
mountC.type = W_TYPE.SECONDARY
mountC.location = Coords(0, -1, 0)
mountC.weapon = Proto.getWeapon("Spike")

ship = GameObject()

ship.mass = 800

ship.size = 15

ship.rateL = 10
ship.dragL = 1.2

ship.rateR = 6
ship.dragR = 3

ship.relativeLookAt = Vector3(0,0,-300)
ship.relativeLookFrom = Vector3(0,8,45)

ship.maxHealth = 500;
ship.maxShield = 200;
ship.shieldIncreaseRate = 20;

ship.models:Add(shipModel)
ship.models:Add(shipModel2)
ship.shieldModel = shieldModel

ship.mounts:Add(mountR)
ship.mounts:Add(mountL)
ship.mounts:Add(mountC)

Proto.ship:Add("Raptor", ship)
