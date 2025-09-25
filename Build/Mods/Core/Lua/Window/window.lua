--- 这里定义了游戏路径如何获取
local Sango = Sango;
---@class Window
local Window = {}
---------------------------------------------------------------------------
---
function Window:Show()
    if self.Behaviour ~= nil then
        self.Behaviour:Show();
    end
end

function Window:Hide()
    if self.Behaviour ~= nil then
        self.Behaviour:Hide();
    end
end

local TypeOfUIText = typeof(UnityEngine.UI.Text);
local TypeOfUIButton = typeof(UnityEngine.UI.Button);
local TypeOfUIImage = typeof(UnityEngine.UI.Image);
local TypeOfUIRawImage= typeof(UnityEngine.UI.RawImage);
local TypeOfSprite= typeof(UnityEngine.Sprite);
local TypeOfTexture= typeof(UnityEngine.Texture);

Window.TypeOfUIText = TypeOfUIText
Window.TypeOfUIButton = TypeOfUIButton
Window.TypeOfUIImage = TypeOfUIImage
Window.TypeOfUIRawImage = TypeOfUIRawImage
Window.TypeOfSprite = TypeOfSprite
Window.TypeOfTexture = TypeOfTexture


function Window:GetText(name)
    if self.Behaviour == nil then
        return ;
    end
    return self.Behaviour:GetComponent(name, TypeOfUIText)
end

function Window:GetImage(name)
    if self.Behaviour == nil then
        return ;
    end
    return self.Behaviour:GetComponent(name, TypeOfUIImage)
end

function Window:GetRawImage(name)
    if self.Behaviour == nil then
        return ;
    end
    return self.Behaviour:GetComponent(name, TypeOfUIRawImage)
end

function Window:GetTransform(name)
    if self.Behaviour == nil then
        return ;
    end
    return self.Behaviour:GetTransform(name)
end

function Window:SetActive(name, b)
   local trans = self:GetTransform(name);
    if trans ~= nil then
        trans.gameObject:SetActive(b);
    end
end

function Window:GetObject(name)
    if self.Behaviour == nil then
        return ;
    end
    return self.Behaviour:GetObject(name)
end

function Window:SetText(name, str)
    local ui = self:GetText(name);
    if ui ~= nil then
        ui.text = str;
    end
end

function Window:SetImage(name, sprite)
    local ui = self:GetImage(name);
    if ui ~= nil then
        ui.sprite = sprite;
    end
end

function Window:SetRawImage(name, tex)
    local ui = self:GetRawImage(name);
    if ui ~= nil then
        ui.texture = tex;
    end
end

function Window:SetImageFillAmount(name, f)
    local ui = self:GetImage(name);
    if ui ~= nil then
        ui.fillAmount = f;
    end
end


return Window;