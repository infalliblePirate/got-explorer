import axios from "axios";
import Cookies from 'universal-cookie';
import { getAuthConfig } from "./GetAuthConfig";
class AuthService {
    cookies = new Cookies(null, { path: '/' });

    login(username: string, password: string) {
        return axios
            .post("api/account/login", { username, password })
            .then((response) => {
                const token = response.data.token;
                const imageId = response.data.imageId;
                if (token) {
                    this.cookies.set('token', token);
                }
                if (imageId) {
                    this.cookies.set('imageId', imageId)
                }
                return response.data;
            });
    }

    logout() {
        this.cookies.remove('gameid');
        this.cookies.remove('levelIds');
        this.cookies.remove('token');
    }

    signup(username: string, email: string, password: string) {
        return axios.post("api/account/register", {
            username,
            email,
            password,
        }).then((response) => {
            const token = response.data.token;
            const imageId = response.data.imageId;
            if (token) {
                this.cookies.set('token', token);
            }
            if (imageId) {
                this.cookies.set('imageId', imageId)
            }
            return response.data;
        });
    }
    update(username: string, email: string) {
        const imageId = this.cookies.get("imageId");
        return axios.put("api/account/update", {
            username,
            email,
            imageId
        },
            {
                headers: { Authorization: `Bearer ${this.cookies.get('token')}` }
            });
    }
    update_password(currentPassword: string, newPassword: string) {
        return axios.put("api/account/update-password", {
            currentPassword,
            newPassword
        }, getAuthConfig());

    }
    update_password(currentPassword: string, newPassword: string) {
        return axios.put("api/account/update-password", {
            currentPassword,
            newPassword
        },
            this.config);

    }
    reset_password(email: string) {
        return axios.put("api/account/password-reset-link", {
            email
        });
    }
    set_new_pass(id: number, password: string, token: string) {
        return axios.put("api/account/password-reset", {
            id,
            password,
            token
        });
    }
    delete() {
        axios.delete("https://localhost:7079/api/account/delete", getAuthConfig());
        this.cookies.remove('gameid');
        this.cookies.remove('levelIds');
        this.cookies.remove('token');
    }
}

export default new AuthService();