import "./GameLevelPage.scss";

export interface Player {
  rank: number;
  name: string;
  score: number;
}

export interface LeaderboardProps {
  players: Player[];
  currentScore: number;
}

const Leaderboard: React.FC<LeaderboardProps> = ({ players, currentScore }) => {
  return (
    <div className="leaderboard">
      <div className="leaderboard__current-score">
            <p>Your points for this game: {currentScore}</p>
      </div>
      <h2 className="leaderboard__title">TOP PLAYERS</h2>
      <table className="leaderboard__table">
        <thead>
          <tr>
            <th className="leaderboard__header">RANK</th>
            <th className="leaderboard__header">NAME</th>
            <th className="leaderboard__header">SCORE</th>
          </tr>
        </thead>
        <tbody>
          {players.map((player) => (
            <tr key={player.rank} className="leaderboard__row">
              <td className="leaderboard__cell">{player.rank}</td>
              <td className="leaderboard__cell">{player.name}</td>
              <td className="leaderboard__cell">{player.score}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default Leaderboard;
