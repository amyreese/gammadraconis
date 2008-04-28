
weapon = Weapon()

weapon.bullet.damage = 50
weapon.bullet.rateL = 800
weapon.bullet.models:Add(FBXModel("Resources/Models/Shell", "", 0.5));

Proto.weapon:Add("Cannon", weapon)