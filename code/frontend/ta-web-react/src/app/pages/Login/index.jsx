import { useState } from "react"
import { login } from "../../services/AuthApiClient";

export function Login () {

    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const handleSubmit = async e => {
        e.preventDefault();
        await login(username, password);
    }

    return (
        <>
            <div>
                <label>Username</label>
                <input type="text" value={username} onChange={e => setUsername(e.target.value)}></input>
            </div>
            <div>
                <label>Password</label>
                <input type="password" value={password} onChange={e => setPassword(e.target.value)}></input>
            </div>
            <div>
                <button type="submit" onClick={handleSubmit}>Login</button>
            </div>
        </>
    )
}