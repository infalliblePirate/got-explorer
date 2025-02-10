import axios from "axios";
import Cookies from 'universal-cookie';
class AuthService {
    cookies = new Cookies(null, { path: '/' });
    login(username: string, password: string) {
        return axios
            .post("api/account/login", { username, password })
            .then((response) => {
                const token = response.data.token;
                if (token) {
                    this.cookies.set('token', token);
                }
                return response.data;
            });
    }

    logout() {
        this.cookies.remove('token');
    }

    signup(username: string, email: string, password: string) {
        return axios.post("api/account/register", {
            username,
            email,
            password,
        }).then((response) => {
            const token = response.data.token;
            if (token) {
                this.cookies.set('token', token);
            }
            return response.data;
        });
    }
}

export default new AuthService();