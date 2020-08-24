# MMapper
三菱製ライブラリMX Componentを手軽に利用する為のラッパークラス

## 概要
PLCのデバイスと変数をマッピングしデバイスへのアクセスを手軽にします。  
UnitTestとの相性も良くC#でPLCプログラムのテストコードを簡単に記述しテスト自動化にも利用可能です。

## PLC論理番号設定
MX Componentで設定した論理番号を指定します。
```C#
PLC.Dev.LogicalNo = 1;
```

## デバイス数設定
デバイスタイプ毎のデバイス数を設定します。
```C#
// 内部デバイスMのデバイス数を9999に変更
PLC.Dev.Length.M = 9999;
```

## ビットデバイスの読み込み
```C#
// XE0
var ret = PLC.Dev.X["E0"].Value;
```

## ビットデバイスの書き込み
```C#
// XE0をON
PLC.Dev.X["E0"].Value = true;
```

## ワードデバイスの読み込み
```C#
// シングルワード（short）
var ret = PLC.Dev.D["0"].SingleValue;

// ダブルワード（int）
var ret = PLC.Dev.D["0"].DoubleValue;

// 単精度浮動小数（float）
var ret = PLC.Dev.D["0"].FloatValue;
```

## ワードデバイスの書き込み
```C#
// シングルワード（short）
PLC.Dev.D["0"].SingleValue = 1234;

// ダブルワード（int）
PLC.Dev.D["0"].DoubleValue = 1234;

// 単精度浮動小数（float）
PLC.Dev.D["0"].FloatValue = 123.4;
