# 何をしているのか?

計算機の環境によって、実数計算の結果に差異があるように思われたので、広汎な調査を行っております。

.Net Core 2.2の実行環境をお持ちで、もしご協力頂けるのなら、[こちら](https://github.com/Tokeiya/RealSurvey/releases/download/Ver1_5/TestExecutorVer1_5.zip)から、プログラムをダウンロードした後、展開して頂き、以下のコマンドを実行してください。

`dotnet <展開されたパス>TestExecutor.dll`

実行後、カレントフォルダに、zipファイルが作成されますので、それを、[こちらのIssue](https://github.com/Tokeiya/RealSurvey/issues)まで投稿して頂けないでしょうか?



# 実行しているテストの内容

実行したテストの内容は以下の通り

1. `[-90°,+90°]`の区間を1°につき128分割してその弧度を計算した。
2. 予め計算されている上記に対応した弧度を元にして、`sin`,`cos`,`tan`及び、`sin/cos`を標準の`System.Math`を使い計算した。
3. [fdlibm](http://www.netlib.org/fdlibm/)　を元にして、Payne-Haneck Algorithmを実装し、同様に`sin`,`cos`,`tan`を計算した。



## 弧度の計算方法

弧度の計算は、以下の二種類を利用した。以下、対応するDegreesをdとする。

1. 単純に`d*(Math.Pi/180)`を計算させた。
2. [jglm](https://github.com/jroyalty/jglm/blob/master/src/main/java/com/hackoeur/jglm/support/FastMath.java#L2983)を参考に倍精度浮動小数点の上位32bitを分割した上で、仮数域を拡張して、高精度計算を行った。

# 実行した環境一覧

今回も多数野ご協力を得る事が出来ました。

有り難うございます。

また、頂いたデータと自前で揃えたデータはとりまとめて、
[SQLite3形式](https://github.com/Tokeiya/RealSurvey/blob/master/Analize/RealSurvey.sqlite3)でここに置いてあります。



| generation          | cpu             | framework              | os                                                           | process_architecture |
| :------------------ | :-------------- | :--------------------- | :----------------------------------------------------------- | :------------------- |
| Ivy Bridge          | i7-3770K        | .NET Core 4.6.27617.05 | Microsoft Windows 10.0.18362                                 | X86                  |
| Ivy Bridge          | i5-3337U        | .NET Core 4.6.27617.05 | Microsoft Windows 10.0.18362                                 | X86                  |
| Haswell             | i5-4200U        | .NET Core 4.6.27617.05 | Microsoft Windows 10.0.18362                                 | X86                  |
| Haswell             | Celeron 2957U   | .NET Core 4.6.27617.05 | Microsoft Windows 10.0.17134                                 | X86                  |
| Skylake             | i3-6006U        | .NET Core 4.6.27617.05 | Microsoft Windows 10.0.17134                                 | X86                  |
| Kaby Lake           | i5-7200U        | .NET Core 4.6.27617.05 | Microsoft Windows 10.0.17134                                 | X86                  |
| Zen                 | Ryzen 7 1700X   | .NET Core 4.6.27617.05 | Microsoft Windows 10.0.18362                                 | X86                  |
| Ivy Bridge          | i7-3770K        | .NET Core 4.6.27617.05 | Microsoft Windows 10.0.18362                                 | X64                  |
| Ivy Bridge          | i5-3337U        | .NET Core 4.6.27617.05 | Microsoft Windows 10.0.18362                                 | X64                  |
| Ivy Bridge          | i7-3770         | .NET Core 4.6.27414.05 | Microsoft Windows 10.0.17763                                 | X64                  |
| Haswell             | Xeon E5-2690 v3 | .NET Core 4.6.27317.07 | Linux 4.15.0-50-generic #54-Ubuntu SMP Mon May 6 18:46:08 UTC 2019 | X64                  |
| Haswell             | i5-4200U        | .NET Core 4.6.27617.05 | Microsoft Windows 10.0.18362                                 | X64                  |
| Haswell             | Celeron 2957U   | .NET Core 4.6.27617.05 | Microsoft Windows 10.0.17134                                 | X64                  |
| Skylake             | i3-6006U        | .NET Core 4.6.27617.05 | Microsoft Windows 10.0.17134                                 | X64                  |
| Kaby Lake           | i7-7567U        | .NET Core 4.6.27617.05 | Darwin 18.6.0 Darwin Kernel Version 18.6.0: Thu Apr 25 23:16:27 PDT 2019; root:xnu-4903.261.4~2/RELEASE_X86_64 | X64                  |
| Kaby Lake           | i5-7200U        | .NET Core 4.6.27617.05 | Microsoft Windows 10.0.17134                                 | X64                  |
| Kaby Lake           | i5-7600         | .NET Core 4.6.27414.05 | Microsoft Windows 10.0.18362                                 | X64                  |
| Kaby Lake           | i7-7567U        | .NET Core 4.6.27617.05 | Darwin 18.6.0 Darwin Kernel Version 18.6.0: Thu Apr 25 23:16:27 PDT 2019; root:xnu-4903.261.4~2/RELEASE_X86_64 | X64                  |
| Coffee Lake         | i7-8086K        | .NET Core 4.6.27414.05 | Microsoft Windows 10.0.18362                                 | X64                  |
| Coffee Lake         | i5-8500B        | .NET Core 4.6.27817.03 | Darwin 18.6.0 Darwin Kernel Version 18.6.0: Thu Apr 25 23:16:27 PDT 2019; root:xnu-4903.261.4~2/RELEASE_X86_64 | X64                  |
| Coffee Lake Refresh | i7-9700K        | .NET Core 4.6.27617.05 | Linux 4.15.0-54-generic #58-Ubuntu SMP Mon Jun 24 10:55:24 UTC 2019 | X64                  |
| Zen                 | Ryzen 7 1700X   | .NET Core 4.6.27617.05 | Microsoft Windows 10.0.18362                                 | X64                  |



# 現時点でわかったこと

### X86とX64で実数計算を行う主体が異なる

X86ではx87 FPUを使い、X64ではSIMD演算を行っていた。



## 64bit版Windowsでの挙動

64bit版Windowsの結果を下にまとめた。

`○`は差異が無かったことを示し、`×`は差異が存在していたことを示している。

![64Win](https://i.imgur.com/q3qbHNy.png)



上記表の様に、Celeronは別とすれば、`Haswell`世代で綺麗に傾向が分かれる。

Celeronの評価だが、2957Uは`Haswell`世代ではあるけど、FMA3命令を持っていない。また、[このような記事](http://math-koshimizu.hatenablog.jp/entry/2017/08/06/151212)を見つけた。

この記事で特筆すべきは、

> **1. 丸め回数が減るので精度的に有利**

とあるので、この辺が絡んでるのでは無いかと思う。



尚、当該記事の紹介と、FMA3命令の有無で計算結果が変わるのではないかというコトを、[@h2rlet ](https://twitter.com/h2rlet)先生に教えて貰った。



## 32bit版Windowsでの挙動

32bit版Windowsにおいて、CPUにおける結果の差異は見られなかった。

但し、[StackOverFlow](https://stackoverflow.com/questions/20963419/cross-platform-floating-point-consistency)で、

> Again, in case of the x87 instruction set, Intel and AMD have historically implemented things a little differently. 

との記載を見つけたので、他の計算をさせた場合、差異が出てくる可能性はある。



![32Win](https://i.imgur.com/Kb4LOV2.png)



## Linuxでの挙動

Linuxにおける挙動は、KernelのVersion、WSLか否か、CPU並びにアーキテクチャの差に拘わらず、常に同じ値を出力した。



![Linux](https://i.imgur.com/4qT5b8b.png)



## OSXでの挙動

Linuxと同様に、OSのVersion、CPUの差異に拘わらず、常に同じ値を出力した。

![osx](https://i.imgur.com/lU6QCe6.png)



## 計算結果に関する分類

以上から、計算結果は以下の様に分類できる

1. 32Bit process Windows
2. 64Bit Process Windows (FMA3未サポートのCPU)
3. 64bit Process Windows(FMA3サポートCPU)
4. 64Bit process Linux(含むWSL)
5. 64Bit process OSX



また、上記分類ごとの差異の有無を調べたところ、以下の用に、全ての分類間で差異が生じる結果となった。

![grp](https://i.imgur.com/CCI46P6.png)

# 

## Linux上にてC言語で生成したデータとの比較



WSL場のLinux(Ubuntu 18.4)にて、clang及びgccのToolchainを使用して、同様の計算をさせてみた。

その結果が以下の表となる。



![langC](https://i.imgur.com/xFjeedh.png)

 

この結果からわかるように、恐らくLinuxのsin,cos,tanにおいては、libcを使用しているnodeは無いかと思われる。



# 今後調べてみたいこと



1. 入力した値で差異の多寡が決まりえるか?
1. Clang/Gcc/CRTの出力結果を調べる
