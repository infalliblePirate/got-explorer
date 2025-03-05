import Cookies from 'universal-cookie';

const cookies = new Cookies();

export const getAuthConfig = () => ({
    headers: { Authorization: `Bearer ${cookies.get('token')}` }
});