export const endpoints = {
  getPosts: (userId: number) => `PostInteraction/suggested/${userId}`,
  addCommentToPost: (userId: number) => `comments/${userId}`,
  getCommentsByPostId: (postId: number) => `Comment/postComments/${postId}`,
}









