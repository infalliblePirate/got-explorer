import "./GameLevelPage.scss";

export interface Player {
    userId: number;
    username: string;
    score: number;
    startTime: string;
    endTime: string;
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

            {players.length > 0 ? (
                <table className="leaderboard__table">
                    <thead>
                        <tr>
                            <th className="leaderboard__header">RANK</th>
                            <th className="leaderboard__header">NAME</th>
                            <th className="leaderboard__header">SCORE</th>
                        </tr>
                    </thead>
                    <tbody>
                        {players.map((player, index) => (
                            <tr key={player.userId} className="leaderboard__row">
                                <td className="leaderboard__cell">{index + 1}</td>
                                <td className="leaderboard__cell">{player.username}</td>
                                <td className="leaderboard__cell">{player.score}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            ) : (
                <p>No leaderboard data available</p>
            )}
        </div>
    );
};

export default Leaderboard;
