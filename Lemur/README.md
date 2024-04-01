# LEMUR

## FrontIST 資料

- 公式マニュアル
  - https://manual.frontistr.com/ja/index.html
- 線形ソルバー解説
  - どのような設定で線形ソルバーを選択すればよいか書かれていてわかりやすい
  - https://www.frontistr.com/seminar/160318/LINEQ_Solver_fixed.pdf
  - METHOD
    - よく使うのは CG と MUMPS
  - PRECONDITION
    - わからないときは SSOR か ML を推奨
  - NIER：最大反復回数
    - 数万回で収束しない場合は設定を見直したほうが良い
  - RESIDUAL
    - 1e-6程度でもよいとあるが、TUTORIAL のファイルを見ると接触とか残差の影響が大きいものは 1e-10 にしている
