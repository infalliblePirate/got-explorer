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
          Dolor sit amet, consectetur adipiscing elit. Etiam laoreet maximus
          metus a fringilla. Vivamus placerat condimentum ultricies malesuada.
          Vivamus placerat eros id pretium dignissim. Suspendisse eget lacus
          malesuada libero auctor tempor eu vehicula tortor.
        </p>
        <button className="game-start-page__button" onClick={handleGameClick}>
          PLAY THE GAME NOW
        </button>
      </div>
    </div>
  );
};

export default GoToGame;
