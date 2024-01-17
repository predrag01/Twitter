import { SyntheticEvent, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

const Login = (props: {setUsername: (username: string) => void, setUserId: (userId: number) => void}) => {

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [redirect, setRedirect] = useState(false);
    const navigate = useNavigate();

    const submit = async (e: SyntheticEvent) => {
        e.preventDefault();

            const response = await fetch('https://localhost:7082' + '/User/Login', {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            credentials: 'include',
            body: JSON.stringify({
                email,
                password
            }), 
            mode:"cors"
        });

        const jwtToken = response.headers.get('jwt');
        console.log(jwtToken)

        if(response.status === 200){
            const respone = await fetch('https://localhost:7082' + '/User/GetUser', {
                    headers: {'Content-Type': 'application/json'},
                    credentials: 'include',
                    mode: 'cors'
            });

                if (respone.status === 200) {
                    const content = await respone.json();
                    props.setUsername(content.username)
                    props.setUserId(content.id)
                    setRedirect(true);
                }
        }
    };

    useEffect(() => {
        if (redirect) {
            navigate('/');
        }
    }, [redirect, navigate]);
    
    return (
        <form className="login" onSubmit={submit}>
            <h1 className="h3 mb-3 fw-normal">Please log in</h1>
            <div className="form-floating login-email">
                <input type="email" className="form-control" placeholder="name@example.com" required onChange={e => setEmail(e.target.value)}/>
                <label >Email address</label>
            </div>
            <div className="form-floating text-start mb-3">
                <input type="password" className="form-control" placeholder="Password" required onChange={e => setPassword(e.target.value)}/>
                <label >Password</label>
            </div>
            <button className="btn btn-primary w-100 py-2" type="submit">Log in</button>
        </form>
    );
};

export default Login;