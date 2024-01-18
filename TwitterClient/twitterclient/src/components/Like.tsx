import { useEffect, useState } from "react";
import { Like } from "../models/like.model";

const LikeComponent = ( props: {postId: number, userId: number}) => {

    const [likes, setLikes] = useState<Like[]>([]);
    const [liked, setLiked] = useState(false);
    const [likeNumber, setLikeNumber] = useState(0);
    const [userId, setUserId] = useState(props.userId);
    const [postId, setPostId] = useState(props.postId);

    useEffect(() => {
        (
          async () => {
            const response = await fetch(`https://localhost:7082/Like/GetLikesByPostId/${encodeURIComponent(props.postId)}`, {
                headers: { 'Content-Type': 'application/json' },
                credentials: 'include',
                mode: 'cors',
            });

            const likeList : Like[] = await response.json();
            setLikes(likeList);
            setLikeNumber(likeList.length);

            const userLiked = likeList.some((like) => like.userId === props.userId);
            setLiked(userLiked);

          }
        )();
    }, [props.postId, props.userId]);

    const like = () => {
        setLikeNumber(likeNumber+1);
        setLiked(true);
        setUserId(userId);
        likePost();
    };

    const unlike = () => {
        setLikeNumber(likeNumber-1);
        setLiked(false);
        setUserId(userId);
        unLikePost();
    };

    const likePost = async () => {
        await fetch('https://localhost:7082' + '/Like/Like', {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            credentials: 'include',
            body: JSON.stringify({
                userId,
                postId
            }),
            mode: 'cors'
        });
    };

    const unLikePost = async () => {
        await fetch('https://localhost:7082' + '/Like/Unlike', {
            method: 'DELETE',
            headers: {'Content-Type': 'application/json'},
            credentials: 'include',
            body: JSON.stringify({
                userId,
                postId
            }),
            mode: 'cors'
        });
    };

    return(
        <div className="like-content">
            <label className="likes">{likeNumber}</label>
            <div>
                {liked ? 
                (<div className="like-button" onClick={unlike}>
                    <i className="bi bi-hand-thumbs-up-fill"></i>
                </div>) : 
                (<div className="like-button" onClick={like}>
                    <i className="bi bi-hand-thumbs-up"></i>
                </div>)}
            </div>
        </div>
    );
};

export default LikeComponent;