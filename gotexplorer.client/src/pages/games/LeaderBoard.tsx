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
    currentUserId: number | null;
}

const Leaderboard: React.FC<LeaderboardProps> = ({ players, currentScore, currentUserId }) => {
    const currentUserIndex = players.findIndex(player => player.userId === currentUserId);
    const topPlayers = players.slice(0, 5);
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
                        {topPlayers.map((player, index) => (
                            <tr key={player.userId} className={`leaderboard__row ${player.userId === currentUserId ? "you" : ""}`}>
                                <td className="leaderboard__cell">{index + 1}</td>
                                <td className="leaderboard__cell">{player.userId === currentUserId ? "You" : player.username}</td>
                                <td className="leaderboard__cell">{player.score}</td>
                            </tr>
                        ))}
                       {currentUserIndex >= 6 && ( 
                            <>
                                <tr>
                                    <td className="leaderboard__cell"></td>
                                    <td className="leaderboard__cell">...</td>
                                    <td className="leaderboard__cell"></td>
                                </tr>
                            </>
                        )}
                        {currentUserIndex >= 5 && (
                            <tr key={players[currentUserIndex].userId} className="leaderboard__row you">
                                <td className="leaderboard__cell">{currentUserIndex + 1}</td>
                                <td className="leaderboard__cell">You</td>
                                <td className="leaderboard__cell">{players[currentUserIndex].score}</td>
                            </tr>
                        )}

                    </tbody>
                </table>
            ) : (
                <p>No leaderboard data available</p>
            )}
        </div>
    );
};

export default Leaderboard;
