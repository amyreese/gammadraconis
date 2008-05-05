checkpoint = GameObject()

modelA = FBXModel("Resources/Models/Checkpoint", "", 0.5)
modelA.lighted = false
checkpoint.models:Add(modelA)

modelB = FBXModel("Resources/Models/Checkpoint2", "", 0.5)
modelB.lighted = false
checkpoint.models:Add(modelB)

checkpoint.size = 100
Proto.thing:Add("Checkpoint", checkpoint)