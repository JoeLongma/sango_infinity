local Window = require "Window/window"
---@class Window_Troop_Bar
---@param self Window_Troop_Bar
Window_Troop_Bar = Class(Window)

function Window_Troop_Bar:Awake(troop)
    self:SetText("bg/name", troop.Name);
    local headRes = string.format("Assets/UI/AtlasTexture/Face/%d_2.png",troop.Leader.headIconID);
    local headIcon = Sango.Loader.ObjectLoader.LoadObject(headRes, Window.TypeOfSprite);
    self:SetImage("bg/head", headIcon);
    self:UpdateState(troop);
end

function Window_Troop_Bar:UpdateState(troop)
    self:SetActive("bg/state", false);
    self:SetImageFillAmount("bg/energyframe/energy", troop.energy / 100);
    self:SetImageFillAmount("bg/angryframe/angry", 0);
    self:SetText("bg/number", troop.troop);
end