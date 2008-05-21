
planet = GameObject()
planet.size = 1000

model = FBXModel("Resources/Models/Planet", "", 4)
model.lighted = false
planet.models:Add(model)

Proto.thing:Add("Mars", planet)