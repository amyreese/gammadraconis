
weapon = Weapon()
weapon.type = W_TYPE.PRIMARY
weapon.cooldown = 100

weapon.bullet.damage = 150
weapon.bullet.rateL = 1000
weapon.bullet.models:Add(FBXModel("Resources/Models/Laser", "", 0.7));
weapon.bullet.dragR = 1

Proto.weapon:Add("Blaster", weapon)