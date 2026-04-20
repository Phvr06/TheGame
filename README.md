# The Game

Projeto individual em Unity desenvolvido para a atividade de introdução rápida ao Unity da disciplina de Jogos Digitais.

## Sobre o jogo

`The Game` é um jogo 2D em que o jogador precisa coletar todas as moedas na ordem correta para liberar o portal de saída antes que o tempo acabe.

Durante a partida:

- o tempo é uma mecânica central que pode resultar na derrota do jogador
- inimigos patrulham o labirinto e fazem o jogador perder vida e tempo
- a interface mostra score, tempo, vidas e a ordem visual das moedas
- o jogo possui menu inicial, HUD, tela de fim de jogo, áudio e suporte a controle/joystick

## Como jogar

Objetivo:

- coletar as 4 moedas na ordem indicada
- evitar os inimigos
- chegar ao portal depois de liberar a saída

Controles:

- `WASD`, setas direcionais ou joystick

Regras importantes:

- as moedas só podem ser coletadas na ordem correta
- encostar em inimigos reduz vidas e tambem remove tempo do cronometro
- a partida termina em vitória ao alcançar o portal liberado
- a partida termina em derrota se o tempo acabar ou se as vidas chegarem a zero

## Estrutura principal

- `Assets/Scenes/MainMenu.unity`: menu inicial
- `Assets/Scenes/SampleScene.unity`: fase principal
- `Assets/GameController.cs`: estado geral da partida
- `Assets/PlayerMovement.cs`: movimentacao, coleta e dano do jogador
- `Assets/OrderedCollectible.cs`: ordem das moedas
- `Assets/MazeEnemy.cs`: comportamento dos inimigos
- `Assets/ExitPortal.cs`: liberacao e uso do portal
- `Assets/UIManager.cs`: HUD e painel de resultado
- `Assets/GameAudioManager.cs`: musica e efeitos sonoros

## Créditos

### Fonte
- fonte: `Assets/Fonts` - https://yukipixels.itch.io/boldpixels

### Áudio

- musica de fundo: `Assets/Audio/Music/Sunken Heart City  - Cody O'Quinn - 01 Sunken Heart City .wav` - https://youtu.be/ZF-CPF_Ndm4
- efeitos sonoros:
  - `Assets/Audio/SFX/coin.mp3` - https://pixabay.com/sound-effects/film-special-effects-coin-recieved-230517/
  - `Assets/Audio/SFX/win.mp3` - https://pixabay.com/sound-effects/film-special-effects-winner-game-sound-404167/
  - `Assets/Audio/SFX/lose.mp3` - https://pixabay.com/sound-effects/film-special-effects-fail-234710/
  - `Assets/Audio/SFX/0419 (1).mp3` - https://pixabay.com/sound-effects/people-suffering-damage-284365/

### Sprites

- jogador: `Assets/Sprites/Player/` - Gerado pelo GPT
- inimigos: `Assets/Sprites/Enemies/` - Gerado pelo GPT
- moedas, coracao: `Assets/Sprites/Other/` - https://greatdocbrown.itch.io/coins-gems-etc
- portal: `Assets/Sprites/Other/` - https://opengameart.org/content/portals-32-x-48
- tiles: `Assets/Sprites/Other/` - https://xanderwood.itch.io/castle-wall-tileset
