import axios from "axios";
import Cookies from "universal-cookie";

class GameService{
    cookies = new Cookies(null, { path: '/' });
    config = {
        headers: { Authorization: `Bearer ${this.cookies.get('token')}` }
    };
     start_game() {
        return  axios
            .post("https://localhost:7079/api/game/start", null, this.config)
            .then((response) => {
                this.cookies.remove("gameid");
                this.cookies.set("gameid", response.data.gameId);
                this.cookies.set("levelIds", response.data.levelIds);
                return response.data;
            });
    }
    load_level(id:number) {
        return axios.get(`https://localhost:5173/api/level/${id}`);
    }
     calculate_level(levelId: number, x: number, y: number) {
         const gameid = this.cookies.get("gameid");
         console.log(gameid);
         return axios.put(`https://localhost:7079/api/game/${gameid}/calculatescore`, {
            levelId,
            x,
            y
        }, this.config);
    }
     complete_game() {
        const gameid = this.cookies.get("gameid");
        return  axios.put(`https://localhost:7079/api/game/${gameid}/complete`,
            null,
            this.config).then(() => {
                this.cookies.remove("gameid");
                this.cookies.remove("levelIds");
            });
    }
    getLeaderboard() {
        console.log("Making GET request to leaderboard");
        return axios.get("https://localhost:7079/api/leaderboard", this.config);
    }
}
export default new GameService();