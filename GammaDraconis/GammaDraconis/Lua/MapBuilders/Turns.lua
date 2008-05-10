--[[
The Turns.lua file creates a series of turns.

QUARTER TURNS
The following turns last a quarter of a circle.  The first heading illustrates the heading a craft will have entering the turn.  The second
headding illustrates the heading a craft will have exiting the turn.  So PosZToNegY is a turn that the user enters facing along the positive Z axis
and exits facing along the negative Y axis.

]]--


function PosZToPosY( path, xoffset, yoffset, zoffset, trackAttributes )
	for i = 0, 120  - trackAttributes.degreesBetweenCheckpoints, trackAttributes.degreesBetweenCheckpoints do
		local rad = MathHelper.ToRadians(i)
		local y = -MSMath.Cos(rad)
		local z = MSMath.Sin(rad)
		table.insert( path, {x=(0) + xoffset, y=(y*trackAttributes.radius) + yoffset, z=(z*trackAttributes.radius) + zoffset, pitch=MSMath.PI - rad, yaw=0, roll=0} )
	end
	
	return path
end

function PosYToPosZ( path, xoffset, yoffset, zoffset, trackAttributes )
	for i = 270, 390  - trackAttributes.degreesBetweenCheckpoints, trackAttributes.degreesBetweenCheckpoints do
		local rad = MathHelper.ToRadians(i)
		local y = MSMath.Cos(rad)
		local z = MSMath.Sin(rad)
		table.insert( path, {x=(0) + xoffset, y=(y*trackAttributes.radius) + yoffset, z=(z*trackAttributes.radius) + zoffset, pitch= rad - MSMath.PI, yaw=0, roll=0} )
	end
	
	return path
end

function PosZToNegX( path, xoffset, yoffset, zoffset, trackAttributes )
for i = 0, 120  - trackAttributes.degreesBetweenCheckpoints, trackAttributes.degreesBetweenCheckpoints do
		local rad = MathHelper.ToRadians(i)
		local x = MSMath.Cos(rad)
		local z = MSMath.Sin(rad)
		table.insert( path, {x=(x*trackAttributes.radius) + xoffset, y=(0) + yoffset, z=(z*trackAttributes.radius) + zoffset, pitch=0, yaw=MSMath.PI-rad, roll=0} )
	end
	
	return path
end

function PosZToPosX( path, xoffset, yoffset, zoffset, trackAttributes )
for i = 0, 120  - trackAttributes.degreesBetweenCheckpoints, trackAttributes.degreesBetweenCheckpoints do
		local rad = MathHelper.ToRadians(i)
		local x = -MSMath.Cos(rad)
		local z = MSMath.Sin(rad)
		table.insert( path, {x=(x*trackAttributes.radius) + xoffset, y=(0) + yoffset, z=(z*trackAttributes.radius) + zoffset, pitch=0, yaw=rad - MSMath.PI, roll=0} )
	end
	
	return path
end

function NegZtoPosY( path, xoffset, yoffset, zoffset, trackAttributes )
	for i = 0, 120  - trackAttributes.degreesBetweenCheckpoints, trackAttributes.degreesBetweenCheckpoints do
		local rad = MathHelper.ToRadians(i)
		local y = -MSMath.Cos(rad)
		local z = -MSMath.Sin(rad)
		table.insert( path, {x=(0) + xoffset, y=(y*trackAttributes.radius) + yoffset, z=(z*trackAttributes.radius) + zoffset, pitch=rad, yaw=0, roll=0} )
	end
	
	return path
end

function NegYToNegZ( path, xoffset, yoffset, zoffset, trackAttributes )
	for i = 91, 200  - trackAttributes.degreesBetweenCheckpoints, trackAttributes.degreesBetweenCheckpoints do
		local rad = MathHelper.ToRadians(i)
		local y = MSMath.Cos(rad)
		local z = MSMath.Sin(rad)
		table.insert( path, {x=(0) + xoffset, y=(y*trackAttributes.radius) + yoffset, z=(z*trackAttributes.radius) + zoffset, pitch=rad - MSMath.PI, yaw=0, roll=0} )
	end
	
	return path
end

function NegYToPosZ( path, xoffset, yoffset, zoffset, trackAttributes )
	for i = 91, 200 - trackAttributes.degreesBetweenCheckpoints, trackAttributes.degreesBetweenCheckpoints do
		local rad = MathHelper.ToRadians(i)
		local y = MSMath.Cos(rad)
		local z = -MSMath.Sin(rad)
		table.insert( path, {x=(0) + xoffset, y=(y*trackAttributes.radius) + yoffset, z=(z*trackAttributes.radius) + zoffset, pitch=-rad, yaw=0, roll=0} )
	end
	
	return path
end

