
const Home = (props: {username:string, userId: number}) => {
    return (
        <div className="home-div">
            <div className="home">
                {props.username ? 'Hi ' + props.username : 'You are not loged in'}
            </div>
        </div>
    );
};

export default Home;