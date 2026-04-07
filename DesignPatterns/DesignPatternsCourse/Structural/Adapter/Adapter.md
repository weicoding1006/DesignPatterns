# Adapter 模式 (Adapter Pattern)

## 1. 什麼是 Adapter 模式？

Adapter (轉接器/適配器) 是一種**結構型設計模式** (Structural Design Pattern)。它的主要目的是**讓介面不相容的類別能夠一起運作**。

就像現實生活中，如果我們去國外旅行，因為國外的插座形狀跟我們自帶的電器插頭不合（比如兩孔與三孔、圓孔與扁孔之分），這時我們會買一個「萬國轉接頭」。這個轉接頭的作用，就是讓我們的插頭（客戶端期望的介面）能成功接上牆壁的插座（現成的類別）。

在軟體開發上，我們經常會遇到類似的情況：我們有一個已開發完成且運作良好的功能（或引入第三方的 DLL 函式庫），但是它提供的 API 介面並不符合我們當前的系統標準。這時我們不需要去直接修改該功能（也許是沒源碼、或為了符合開閉原則），而是**建立一個「適配器 (Adapter)」做為中間層**進行轉換。

## 2. 適用的情境

1. **整合第三方函式庫或舊有系統**：當你想使用某些既有的類別，但對方的介面與你的程式碼標準不相容時。
2. **重用不再維護的舊類別**：你想復用一些從前的類別，而且不希望因為這些舊介面去弄髒目前新的系統架構。
3. **介面轉換**：統一提供一種介面風格，以供不同的系統組件一起工作。

## 3. 參與的角色

Adapter 模式通常有以下三個核心角色：

1. **Target (目標介面)**：客戶端（Client）所期待使用的標準介面。
2. **Adaptee (被適配者)**：已經存在、有實際功能的類別，但它的介面與 `Target` 不相容。這就是我們需要「被轉接」的對象。
3. **Adapter (適配器)**：將 `Adaptee` 包裝起來，並實作 `Target` 的介面。當客戶端呼叫 `Target` 的方法時，`Adapter` 在內部會「翻譯」並去呼叫 `Adaptee` 的對應功能。

### 兩種 Adapter 模式的實作方式

- **物件適配器 (Object Adapter)**：最常用！透過**組合 (Composition)**，Adapter 中包含一個 Adaptee 的實例，並呼叫該實例的方法。我們這個專案範例使用的是這一個。
- **類別適配器 (Class Adapter)**：透過**多重繼承 (Multiple Inheritance)**，Adapter 同時繼承了 Target 的介面與 Adaptee 的實作。但在 C# 中不支援多重繼承其他類別，所以通常是繼承一個類別同時實作一個介面。

## 4. 程式碼範例結構解析

在左側的範例原始碼中，我們模擬了以下情境：

> 我們的現代資料分析系統**只能接收 JSON 格式資料** （`IJsonDataProvider`）。
> 但是，我們需要接入另一個**舊有的資料提供系統**，它只會吐出 XML 格式資料 （`LegacyXmlDataReader`）。

* **`IJsonDataProvider` (Target)**：定義了 `GetJsonData()` 方法。
* **`LegacyXmlDataReader` (Adaptee)**：定義了舊有的 `GetXmlData()`，吐出的是 XML 字串。
* **`XmlToJsonAdapter` (Adapter)**：實作 `IJsonDataProvider`。內部持有 `LegacyXmlDataReader` 物件，當被呼叫 `GetJsonData()` 時，會先呼叫舊有的 XML 讀取方法，隨後將它即時轉換為 JSON 格式，回傳給呼叫方。

## 5. 如何將它整合到 Program.cs？

如果您想觀察它的執行結果，可以在您的 `Program.cs` 裡加入以下測試代碼：

```csharp
using DesignPatterns.DesignPatternsCourse.Structural.Adapter;
using System;

Console.WriteLine("=== Adapter Pattern ===");

// 1. 建立一個舊有系統 (Adaptee) 的實例
LegacyXmlDataReader legacySystem = new LegacyXmlDataReader();

// 2. 舊系統因為只能吐出 XML，正常無法直接接到新系統上
// 建立一個適配器 (Adapter)，並將舊系統當作參數傳入進行封裝
IJsonDataProvider adapter = new XmlToJsonAdapter(legacySystem);

// 3. 客戶端 (Client) 只需認識 Target 介面 (IJsonDataProvider) 並調用方法
string resultJson = adapter.GetJsonData();

Console.WriteLine($"\n[Client] 成功取得最終資料:\n{resultJson}");
```

## 6. 優點與缺點

### 優點
- **符合單一責任原則 (SRP)**：你可以將資料轉接或格式轉換等邏輯，從主要的商業邏輯中分離出來，專注寫在 Adapter 裡面。
- **符合開閉原則 (OCP)**：你可以在不修改既有程式碼（特別是客戶端代碼與 Adaptee）的前提下，向程式中引入新型別的適配器。

### 缺點
- **增加程式碼複雜度**：有時候如果需要轉換的介面其實很簡單，只為了轉接就增加一堆新的類別與介面，會讓整體程式變得稍微複雜。在這種極端情況下，直接修改原始程式碼說不定會更單純（前提是你擁有源碼且允許修改）。
