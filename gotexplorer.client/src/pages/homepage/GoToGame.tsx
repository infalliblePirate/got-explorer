import { useNavigate } from "react-router-dom";

const GoToGame = () => {
    const navigate = useNavigate();
    const handleGameClick = () => navigate("/startgame");
    return (
        <button className='start-btn' onClick={handleGameClick}>
            START NOW
        </button>
    );
}
export default GoToGame;