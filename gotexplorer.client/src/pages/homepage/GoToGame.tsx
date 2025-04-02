import { useNavigate } from "react-router-dom";

const GoToGame = () => {
  const navigate = useNavigate();

  const handleGameClick = () => {
    navigate("/startgame");
  };

  return (
    <div className="game-start-page">
      <div className="game-start-page__content">
        <p>
          In the great game, as in Westeros, you only get one move a day. One
          decision. One chance. Each morning, a new challenge rises — as
          unpredictable as the winds beyond the Wall and as cunning as a
          Lannister’s smile. <br /> <br /> When you play the game of thrones,
          you win or you die.” Well... maybe not die.{" "}
          <i>But you’ll definitely have to wait ‘til tomorrow.</i>
        </p>
        <button className="game-start-page__button" onClick={handleGameClick}>
          PLAY THE GAME NOW
        </button>
      </div>
    </div>
  );
};

export default GoToGame;
